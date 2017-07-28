﻿/**
 * Copyright (C) 2016-2017 Regents of the University of California.
 * @author: Jeff Thompson <jefft0@remap.ucla.edu>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * A copy of the GNU Lesser General Public License is in the file COPYING.
 */

using System;
using System.Threading;
using System.Collections.Generic;

using net.named_data.jndn;
using net.named_data.cnl_dot_net;

namespace TestCnlDotNet {
  /// <summary>
  /// This tests updating a namespace based on segmented content.
  /// </summary>
  class TestSegmented {
    static void
    Main(string[] args)
    {
      Face face = new Face("memoria.ndn.ucla.edu");
      Namespace page = new Namespace
        (new Name
          ("/ndn/edu/ucla/remap/demo/ndn-js-test/named-data.net/project/ndn-ar2011.html/%FDX%DC5B"));
      page.setFace(face);

      bool[] enabled = { true };
      page.addOnContentSet
        (delegate(Namespace nameSpace, Namespace contentNamespace, long callbackId) {
          onContentSet(nameSpace, contentNamespace, callbackId, enabled); });
      SegmentedContent segmentedContent = new SegmentedContent(page);
      segmentedContent.start();

      while (enabled[0]) {
        face.processEvents();
        // We need to sleep for a few milliseconds so we don't use 100% of the CPU.
        Thread.Sleep(10);
      }
    }

    /// <summary>
    /// This is called to print the content after it is re-assembled from
    /// segments.
    /// </summary>
    /// <param name="nameSpace">The calling Namespace.</param>
    /// <param name="contentNamespace">The Namespace where the content was set.
    /// </param>
    /// <param name="callbackId">The callback ID returned by addOnContentSet.
    /// </param>
    /// <param name="enabled">On success or error, set enabled[0] = false.
    /// </param>
    static void
    onContentSet
      (Namespace nameSpace, Namespace contentNamespace, long callbackId,
       bool[] enabled)
    {
      if (contentNamespace == nameSpace) {
        Console.Out.WriteLine
          ("Got segmented content size " + contentNamespace.getContent().size());
        enabled[0] = false;
      }
    }
  }
}